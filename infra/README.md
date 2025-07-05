## Deployment Instructions

1. **Login to Azure**

```bash
az login
```

2. **Set Subscription (if you have multiple subscriptions)**

```bash
az account set --subscription <subscription-id>
```

3. **Deploy the Solution**

```bash
az deployment sub create \
  --name AppServicesDeployment \
  --location eastus \
  --template-file main.bicep \
  --parameters location=eastus rgName=rg-app-services
```

## Parameters

- `location` (default: eastus) - The Azure region for resource deployment
- `rgName` (default: rg-app-services) - The name of the resource group
- `qaLocation` (default: eastus) - The Azure region for the QA environment
- `qaRgName` (default: rg-app-services-qa) - The name of the QA resource group

## Common Issues and Solutions

### Scope Mismatch Error
If you encounter the error: "The target scope 'resourceGroup' does not match the deployment scope 'subscription'", ensure that:

1. Your main.bicep file has `targetScope = 'subscription'` at the top if you're using the `az deployment sub create` command
2. Any nested module that deploys at resource group level should specify the scope explicitly:

```bicep
module appServiceModule 'appServices.bicep' = {
  name: 'appServiceDeployment'
  scope: resourceGroup(rgName)  // This specifies the deployment scope for the module
  params: {
    // parameters
  }
}
```

## Example Deployment Command

```bash
az deployment sub create --name AppServicesDeployment --location westeurope --template-file main.bicep --parameters location=westeurope rgName=dev-nho-rg env=qa
```


