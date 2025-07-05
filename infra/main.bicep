targetScope = 'subscription'
param env string

param location string = 'westeurope'
param rgName string

// Deploy resource group
module resourceGroupModule 'resourceGroup.bicep' = {
  name: 'resourceGroupDeployment'
  scope: subscription()
  params: {
    rgName: rgName
    location: location
  }
}

// Deploy app services
module appServicesModule 'appServices.bicep' = {
  name: 'appServicesDeployment'
  scope: resourceGroup(rgName)
  params: {
    location: location
    env: env
  }
  dependsOn: [
    resourceGroupModule
  ]
}

// Deploy Key Vault
module keyVaultModule 'keyVault.bicep' = {
  name: 'keyVaultDeployment'
  scope: resourceGroup(rgName)
  params: {
    location: location
    backendPrincipalId: appServicesModule.outputs.backendPrincipalId
    keyVaultName: '${env}-nho-keyvault-app'
  }
  dependsOn: [
    resourceGroupModule
  ]
}
 

output backendUrl string = appServicesModule.outputs.backendUrl
output frontendUrl string = appServicesModule.outputs.frontendUrl
output backendStaticIp string = appServicesModule.outputs.backendStaticIp
output keyVaultName string = keyVaultModule.outputs.keyVaultName
