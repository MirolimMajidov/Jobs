echo "Deploying k8s services..."
kubectl create -f MongoDB.yaml
kubectl create -f RabbitMQ.yaml
kubectl create -f PaymentService.yaml
kubectl create -f OcelotApiGatewayService.yaml