echo "Port forwarding k8s mongo db service..."
kubectl port-forward deployment/payment-mongo-deployment 8013:27017