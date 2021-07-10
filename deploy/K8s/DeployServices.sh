
echo "Deleting k8s services..."
kubectl delete -f MongoDB.yaml
kubectl delete -f RabbitMQ.yaml
kubectl delete -f PaymentService.yaml
kubectl delete -f OcelotApigatewayService.yaml

echo "Applying k8s services..."
kubectl create -f MongoDB.yaml
kubectl create -f RabbitMQ.yaml
kubectl create -f PaymentService.yaml
kubectl create -f OcelotApigatewayService.yaml