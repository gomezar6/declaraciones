$rgName = "RG-PDLA-CR"
$functionAppName = "azfun-pdla-cr-01"
$functionAppFile = "drop.zip"
az login 
# az functionapp deployment source config-zip -g $rgName -n $functionAppName --src $functionAppFile 






$rgName = "RG-PDLA-CR"
$webAppName = "azapp-pdla-cr-01"
$webAppFile = "drop.zip"
az login 
az functionapp deployment source config-zip -g $rgName -n $webAppName --src $webAppFile 





#az functionapp deployment source config-zip -g "RG-PDLA-CR" -n "azfun-pdla-cr-01" --src "drop.zip"

#az functionapp deployment source config-zip -g "RG-PDLA-CR" -n "azapp-pdla-cr-01" --src "drop.zip"