import Http from "../utils/HttpClient";
import {sleep} from "../utils/CommonUtils";
import "./MockData";
export default{
    getApiInfo:async ()=>{
        var response=await Http.get("/api/api-info");
        await sleep(1000);
        if(response && response.data){
            return response.data;
        }
        else{
            return {success:false,message:"Server Error"}
        }
    }
}