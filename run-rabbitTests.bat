@echo off
dotnet test --filter TestCategory=rabbitmq_test --logger trx
dotnet run trxlog2html -i "TestResults\report1.trx" -o "TestResults\HTMLreport"
trx-to-html