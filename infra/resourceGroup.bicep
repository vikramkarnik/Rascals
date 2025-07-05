targetScope = 'subscription'

param rgName string
param location string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
}

output resourceGroupId string = resourceGroup.id
output resourceGroupName string = resourceGroup.name
