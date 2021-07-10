
echo "Deleting k8s services..."
kubectl delete -f services.yaml
kubectl delete -f deployments.yaml

echo "Applying k8s services..."
kubectl create -f services.yaml
kubectl create -f deployments.yaml