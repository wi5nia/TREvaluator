#
# Swap_AppServiceSlot.ps1
#


Param(
    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$ResourceGroupName,

    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$sourceSlot,

    [Parameter(Mandatory = $true)]
    [ValidatePattern("^[a-zA-Z0-9-_]*$")]
    [String]$targetSlot,

    [Parameter(Mandatory = $true)]
    [String]$WebAppName
)


$ParametersObject = @{targetSlot  = "$targetSlot"}
$ResourceName = "$WebAppName/$sourceSlot"
Invoke-AzureRmResourceAction -ResourceGroupName $ResourceGroupName -ResourceType Microsoft.Web/sites/slots -ResourceName $ResourceName -Action slotsswap -Parameters $ParametersObject -ApiVersion 2015-07-01