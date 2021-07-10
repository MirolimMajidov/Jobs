echo "Deleting k8s services..."
kubectl delete -f PaymentService.yaml
kubectl delete -f OcelotApiGatewayService.yaml
kubectl delete -f RabbitMQ.yaml
kubectl delete -f MongoDB.yaml