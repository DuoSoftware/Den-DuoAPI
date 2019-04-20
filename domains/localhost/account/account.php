<?php

return function ($context){
    $request = $context->getRequest();
    //echo $request->Body();
    $body=json_decode($request->Body());
    //try{
        require_once(TENANT_RESOURCE_PATH."/bo_lib/master/account.php");
        //var_dump($body);
       
        //var_dump($body);
        $sqlUnit = $context->resolve("mssql:query");
        $Account =new Account($sqlUnit);
        $Accountobject=$Account->get_accountbyvc($request->Params()->vcno);
        $Accountobject->entitlements=$Account->get_entitlements();
        $Accountobject->channels=$Account->get_channels();
        return $Accountobject;
    //}catch(Exception $e){
        //return $e;
    //}
}; 