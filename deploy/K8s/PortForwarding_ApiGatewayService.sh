echo "Port forwarding k8s ocelot apigateway service..."
kubectl port-forward deployment/ocelot-apigateway-deployment 8000:80