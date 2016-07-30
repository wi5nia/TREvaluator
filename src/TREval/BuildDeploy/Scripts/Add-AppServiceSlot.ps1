#
# Add deployment slot to AppService
#

Param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$ResourceGroupName,

    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$WebAppName,

    [Parameter(Mandatory = $true)]
    [String]$Slot
)

Write-Output "Adding $Slot slot to $WebAppName app in $ResourceGroupName resource group."
New-AzureRmWebAppSlot -Name $WebAppName -ResourceGroupName $ResourceGroupName -Slot $Slot