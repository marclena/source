@echo off
"C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\"tcm run /create  /login:vuelingbcn\tfsservice,S3rv1c3s /title:"Regression PRE Full (Nightly)" /planid:2  /settingsname:"PRE" /testenvironment:"PRE" /collection:http://wbcnvuetfs:8080/tfs/vueling/ /teamproject:VUELING /querytext:"SELECT * FROM TestPoint WHERE SuiteId= 104" /include
