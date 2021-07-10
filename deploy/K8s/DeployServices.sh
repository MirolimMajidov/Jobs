
echo "Deleting k8s services..."
kubectl delete -f ocelot-apigateway-service.yaml

echo "Applying k8s services..."
kubectl create -f ocelot-apigateway-service.yaml