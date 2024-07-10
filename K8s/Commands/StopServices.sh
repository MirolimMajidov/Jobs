echo "Deleting Jobs services from K8s..."
kubectl delete -f ../IdentityService.yaml
kubectl delete -f ../JobService.yaml
kubectl delete -f ../PaymentService.yaml
kubectl delete -f ../OcelotApiGatewayService.yaml
kubectl delete -f ../MessageBroker_RabbitMQ.yaml
kubectl delete -f ../Identity_MySQL.yaml
kubectl delete -f ../Job_SQLServer.yaml
kubectl delete -f ../Payment_MongoDB.yaml
kubectl delete -f ../Payment_RedisDB.yaml

echo "Completed"
