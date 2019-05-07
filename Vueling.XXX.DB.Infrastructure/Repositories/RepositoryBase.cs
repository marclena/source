using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel;

namespace Vueling.XXX.DB.Infrastructure.Repositories
{
    public class RepositoryBase<TInput, TOutput> : IDisposable
        where TInput : class
        where TOutput : EntityObject
    {
        private ObjectContext _context;

        public ObjectContext Context
        {
            get { return _context; }
            set { _context = value; }  
        }

        protected IEnumerable<TOutput> Find(string query, System.Data.Objects.ObjectParameter parameters)
        {
            return Context.CreateQuery<TOutput>(query, parameters).AsEnumerable<TOutput>();
        }

        protected TOutput Find(TInput entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            var mapped = factory.GetDbObjectFromEntity<TInput, TOutput>(entity);

            string entityName = GetEntitySetName(mapped.GetType());
            System.Data.EntityKey key = Context.CreateEntityKey(entityName, mapped);

            return Context.GetObjectByKey(key) as TOutput;
        }

        protected TOutput Create(TInput entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            var mapped = factory.GetDbObjectFromEntity<TInput, TOutput>(entity);

            string entityName = GetEntitySetName(mapped.GetType());

            Context.AddObject(entityName, mapped);

            return mapped;
        }

        protected IEnumerable<TOutput> CreateCollection(IEnumerable<TInput> entities)
        {
            if (entities == null || entities.Count() == 0)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            IEnumerable<TOutput> mappedIEnumerable = factory.GetDbObjectsFromEntities<TInput, TOutput>(entities);

            string entityName = GetEntitySetName(mappedIEnumerable.First().GetType());

            mappedIEnumerable.ToList().ForEach(entity => Context.AddObject(entityName, entity));

            return mappedIEnumerable;
        }

        protected TOutput Delete(TInput entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            var mapped = factory.GetDbObjectFromEntity<TInput, TOutput>(entity);

            mapped = GetOrAttachDbObject(mapped);

            Context.DeleteObject(mapped);

            return mapped;
        }

        protected IEnumerable<TOutput> DeleteCollection(IEnumerable<TInput> entities)
        {
            if (entities == null || entities.Count() == 0)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            IEnumerable<TOutput> mappedIEnumerable = factory.GetDbObjectsFromEntities<TInput, TOutput>(entities);

            foreach (var entity in mappedIEnumerable)
            {
                var mapped = GetOrAttachDbObject(entity);
                Context.DeleteObject(mapped);

            }

            return mappedIEnumerable;
        }

        protected virtual TOutput Update(TInput entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            var mapped = factory.GetDbObjectFromEntity<TInput, TOutput>(entity);

            string entityName = GetEntitySetName(mapped.GetType());
            System.Data.EntityKey key = Context.CreateEntityKey(entityName, mapped);
            object originalItem;

            if (Context.TryGetObjectByKey(key, out originalItem))
            {
                Context.ApplyCurrentValues(key.EntitySetName, mapped);
            }

            return mapped;

        }

        protected virtual IEnumerable<TOutput> UpdateCollection(IEnumerable<TInput> entities)
        {
            if (entities == null || entities.Count() == 0)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
            IEnumerable<TOutput> mappedIEnumerable = factory.GetDbObjectsFromEntities<TInput, TOutput>(entities);

            string entityName = GetEntitySetName(mappedIEnumerable.First().GetType());

            foreach (var entity in mappedIEnumerable)
            {
                System.Data.EntityKey key = Context.CreateEntityKey(entityName, entity);
                object originalItem;

                if (Context.TryGetObjectByKey(key, out originalItem))
                {
                    Context.ApplyCurrentValues(key.EntitySetName, entity);
                }
            }

            return mappedIEnumerable;
        }

        protected int Persist(object entityOrCollection)
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (OptimisticConcurrencyException)
            {
                Context.Refresh(System.Data.Objects.RefreshMode.ClientWins, entityOrCollection);
                return Context.SaveChanges();
            }
        }

        protected virtual void Rollback(object entityOrCollection)
        {
            if (entityOrCollection == null)
            {
                throw new ArgumentNullException("Entity to be created can not be null");
            }

            if (entityOrCollection.GetType() == typeof(IEnumerable<object>).GetGenericTypeDefinition())
            {
                List<TOutput> mapped = new List<TOutput>();

                foreach (var entity in entityOrCollection as IEnumerable<object>)
                {
                    MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
                    var mappedItem = factory.GetDbObjectFromEntity<TInput, TOutput>(entity as TInput);
                    mappedItem = GetOrAttachDbObject(mappedItem);
                    mapped.Add(mappedItem);
                    Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, mapped);
                }
            }
            else
            {
                MapDomainToConceptualModelFactoryBase factory = SwitcherEntityToRepositoryFactory.GetFactoryFor(typeof(TInput), typeof(TOutput));
                var mapped = factory.GetDbObjectFromEntity<TInput, TOutput>(entityOrCollection as TInput);
                mapped = GetOrAttachDbObject(mapped);
                Context.Refresh(System.Data.Objects.RefreshMode.StoreWins, mapped);
            }

        }

        protected System.Data.Objects.ObjectQuery<TOutput> CreateQuery(bool withTracking)
        {
            string entityName = GetEntitySetName();

            System.Data.Objects.ObjectQuery<TOutput> query = Context.CreateQuery<TOutput>(entityName);

            if (!withTracking) { query.MergeOption = System.Data.Objects.MergeOption.NoTracking; }

            return query;
        }

        private string GetEntitySetName()
        {
            string entityName = typeof(TOutput).Name;

            var entityContainer = Context.MetadataWorkspace.GetEntityContainer(Context.DefaultContainerName, DataSpace.CSpace);

            return entityContainer.BaseEntitySets.Where(meta => meta.ElementType.Name == entityName).Select(meta => meta.Name).FirstOrDefault();
        }

        private string GetEntitySetName(Type entityType)
        {
            string entityName = entityType.Name;

            var entityContainer = Context.MetadataWorkspace.GetEntityContainer(Context.DefaultContainerName, DataSpace.CSpace);

            return entityContainer.BaseEntitySets.Where(meta => meta.ElementType.Name == entityName).Select(meta => meta.Name).FirstOrDefault();
        }

        private TOutput GetOrAttachDbObject(TOutput mapped)
        {
            ObjectStateEntry entry;
            string entityName = GetEntitySetName(mapped.GetType());
            System.Data.EntityKey key = Context.CreateEntityKey(entityName, mapped);
            mapped.EntityKey = key;

            if (!Context.ObjectStateManager.TryGetObjectStateEntry(key, out entry)) Context.Attach(mapped);
            else mapped = Context.GetObjectByKey(key) as TOutput;
            return mapped;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
