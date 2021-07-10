echo "Port forwarding k8s payment service..."
kubectl port-forward deployment/payment-api-deployment 8003:80