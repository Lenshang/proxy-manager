import Http from "../utils/HttpClient";
import {sleep} from "../utils/CommonUtils";
import "./MockData";
export default {
    get_all_nodes:async ()=>{
        var response=await Http.get("/api/nodemanager/get-all-nodes");
        if(response.data.success){
            return response.data.data;
        }
    },
    re_dial:async (id)=>{
        var response=await Http.get("/api/nodemanager/re-dial?id="+id);
        await sleep(1000);
        if(response.data.success){
            return response.data.success;
        }
    }
};