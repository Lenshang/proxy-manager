import Http from "../utils/HttpClient";
import {sleep} from "../utils/CommonUtils";
import "./MockData";
export default {
    checkLogin:async ()=>{
        var response=await Http.get("/api/check-login");
        if(response.data.success){
            return response.data.success;
        }
    }
}