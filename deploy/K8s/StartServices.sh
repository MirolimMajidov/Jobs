echo "Deploying k8s services..."
kubectl create -f MongoDB.yaml
kubectl create -f SQLServer.yaml
kubectl create -f MySQL.yaml
kubectl create -f RabbitMQ.yaml
kubectl create -f PaymentService.yaml
kubectl create -f IdentityService.yaml
kubectl create -f JobService.yaml
kubectl create -f OcelotApiGatewayService.yaml

sleep 2
echo "Completed"
