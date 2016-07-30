#
# Add secret to KeyVault
#

Param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$VaultName,

    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$Name,

    [Parameter(Mandatory = $true)]
    [String]$Secret,
	
    [Parameter(Mandatory = $true)]
    [String]$SPN,
	
    [Parameter(Mandatory = $false)]
	[ValidateSet("Software","HSM")] 
    [String]$Destination = 'Software'
)

Set-AzureRmKeyVaultAccessPolicy -ServicePrincipalName $SPN -VaultName $VaultName -PermissionsToKeys all -PermissionsToSecrets all
$SecretSecureString = ConvertTo-SecureString $Secret -AsPlainText -Force
Set-AzureKeyVaultSecret -VaultName $VaultName -Name $Name -SecretValue $SecretSecureString
Remove-AzureRmKeyVaultAccessPolicy -ServicePrincipalName $SPN -VaultName $VaultName