echo "Running Jobs services on K8s..."
kubectl apply -f ../MessageBroker_RabbitMQ.yaml
kubectl apply -f ../Identity_MySQL.yaml
kubectl apply -f ../IdentityService.yaml
kubectl apply -f ../Job_SQLServer.yaml
kubectl apply -f ../JobService.yaml
kubectl apply -f ../Payment_MongoDB.yaml
kubectl apply -f ../Payment_RedisDB.yaml
kubectl apply -f ../PaymentService.yaml
kubectl apply -f ../OcelotApiGatewayService.yaml

echo "Completed"
timeout /t 10 /nobreak
