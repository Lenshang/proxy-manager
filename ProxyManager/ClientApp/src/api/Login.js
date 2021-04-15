import Http from "../utils/HttpClient";
import {sleep} from "../utils/CommonUtils";
import "./MockData";
export default {
    login:async (params)=>{
        var response=await Http.post("/api/auth/login",params);
        await sleep(1000);
        if(response && response.data){
            return response.data;
        }
        else{
            return {success:false,message:"Server Error"}
        }
    }
};