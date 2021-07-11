echo "Port forwarding k8s rabbitmq service..."
kubectl port-forward deployment/rabbitmq-deployment 8014:15672