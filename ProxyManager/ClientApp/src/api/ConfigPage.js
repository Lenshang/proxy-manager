import Http from "../utils/HttpClient";
import {sleep} from "../utils/CommonUtils";
import "./MockData";
export default{
    getConfig:async ()=>{
        var response=await Http.get("/api/get-config");
        await sleep(1000);
        if(response && response.data){
            return response.data;
        }
        else{
            return {success:false,message:"Server Error"}
        }
    },
    saveConfig:async (params)=>{
        var response=await Http.post("/api/save-config",params);
        await sleep(1000);
        if(response && response.data){
            return response.data;
        }
        else{
            return {success:false,message:"Server Error"}
        }
    }
}