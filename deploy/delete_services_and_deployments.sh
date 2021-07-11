echo "Deleting k8s services and deployments..."
kubectl delete deployments --all
kubectl delete services --all
