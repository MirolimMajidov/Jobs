echo "Deleting k8s services..."
kubectl delete -f PaymentService.yaml
kubectl delete -f IdentityService.yaml
kubectl delete -f JobService.yaml
kubectl delete -f OcelotApiGatewayService.yaml
kubectl delete -f RabbitMQ.yaml
kubectl delete -f MongoDB.yaml
kubectl delete -f SQLServer.yaml
kubectl delete -f MySQL.yaml

sleep 2
echo "Completed"
