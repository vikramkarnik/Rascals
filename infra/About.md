
1. main.bicep
Purpose:
Acts as the entry point for your deployment. It orchestrates the deployment of the resource group, app services, and Key Vault by referencing the other Bicep modules.
Key Points:
•	Parameters:
•	env: The environment name (e.g., dev, prod).
•	location: Azure region (default: uksouth).
•	rgName: Name of the resource group.
•	Modules:
•	resourceGroupModule: Deploys the resource group using resourceGroup.bicep.
•	appServicesModule: Deploys the backend App Service, App Service Plan, VNet, NAT Gateway, and Public IP using appServices.bicep.
•	keyVaultModule: Deploys the Azure Key Vault using keyVault.bicep, passing the managed identity principal ID from the backend app for access policy.
•	Outputs:
•	Exposes the backend URL, static IP, and Key Vault name for use after deployment.

2. resourceGroup.bicep
Purpose:
Creates an Azure Resource Group, which acts as a logical container for all other resources.
Key Points:
•	Parameters:
•	rgName: Name of the resource group.
•	location: Azure region.
•	Resource:
•	Deploys a resource group with the specified name and location.
•	Outputs:
•	Returns the resource group’s ID and name.

3. appServices.bicep
Purpose:
Deploys the backend infrastructure required for your .NET 8 application.
Key Points:
•	Parameters:
•	location: Azure region.
•	env: Environment name (used in resource naming).
•	Resources:
•	Virtual Network (VNet):
•	Provides network isolation and enables integration with the App Service.
•	Contains a subnet (be-subnet) delegated to App Service.
•	Subnet is associated with a NAT Gateway for outbound static IP.
•	Public IP Address:
•	Static IP for outbound traffic.
•	NAT Gateway:
•	Ensures all outbound traffic from the subnet uses the static public IP.
•	App Service Plan:
•	Windows-based, Standard S1 tier, hosts the App Service.
•	App Service:
•	.NET 8 backend app, system-assigned managed identity, integrated with the VNet.
•	Configured for HTTP/2, minimum TLS 1.2, and VNet routing.
•	Outputs:
•	Backend app URL, static IP, and the principal ID of the app’s managed identity (used for Key Vault access).

4. keyVault.bicep (optional in this scenario, must for any real world application)
Purpose:
Deploys an Azure Key Vault for secure storage of secrets, keys, and certificates.
Key Points:
•	Parameters:
•	location: Azure region.
•	backendPrincipalId: The managed identity principal ID of the backend app (for access policy).
•	keyVaultName: Name of the Key Vault.
•	Resource:
•	Key Vault:
•	Standard SKU, enabled for deployment, template deployment, and disk encryption.
•	Access policy grants the backend app’s managed identity get and list permissions for secrets.
•	RBAC authorization is disabled (using access policies instead).
•	Outputs:
•	Key Vault name and URI.
