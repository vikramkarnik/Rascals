param location string
param env string


// Create a virtual network for the backend app service
resource vnet 'Microsoft.Network/virtualNetworks@2021-05-01' = {
  name: 'vnet-backend'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    subnets: [
      {
        name: 'be-subnet'
        properties: {
          addressPrefix: '10.0.1.0/24'
          delegations: [
            {
              name: 'delegation'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          natGateway: {
            id: backendNatGateway.id
          }
        }
      }
    ]
  }
}

// Public IP for backend - will be static
resource backendPublicIP 'Microsoft.Network/publicIPAddresses@2021-05-01' = {
  name: 'pip-backend'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

// NAT Gateway for outbound traffic with static IP
resource backendNatGateway 'Microsoft.Network/natGateways@2021-05-01' = {
  name: 'nat-backend'
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIpAddresses: [
      {
        id: backendPublicIP.id
      }
    ]
    idleTimeoutInMinutes: 4
  }
}

// Backend App Service Plan (Windows)
resource backendServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'asp-backend'
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
  }
}

// Backend App Service (.NET 8)
resource backendApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${env}-rascals-backend-app'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: backendServicePlan.id
    virtualNetworkSubnetId: vnet.properties.subnets[0].id
    vnetRouteAllEnabled: true
    siteConfig: {
      netFrameworkVersion: 'v8.0'  // Using .NET 8 for Windows
      http20Enabled: true
      minTlsVersion: '1.2'
      vnetRouteAllEnabled: true
      // Removed ipSecurityRestrictions to allow access from anywhere
    }
  }
}


output backendUrl string = 'https://${backendApp.properties.defaultHostName}'
output backendStaticIp string = backendPublicIP.properties.ipAddress
output backendPrincipalId string = backendApp.identity.principalId
