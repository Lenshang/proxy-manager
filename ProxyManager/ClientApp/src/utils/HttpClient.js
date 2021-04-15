import axios, { AxiosRequestConfig, AxiosResponse, AxiosError } from 'axios';

axios.defaults.withCredentials = true;
axios.defaults.timeout = 6000;

export default class HttpClient {
    static beforeRequest=null;
    static beforeResponse=null;
    static onError=null;
    static async get(url,data=null, other={}) {
        let request = {
            method: 'get',
            url: url,
            ...other
        };
        if(data){
            request.params=data;
        }
        return await this.createRequest(request);
    }

    static async post(url, data=null, other={}) {
        let request = {
            method: 'post',
            url: url,
            data: data,
            ...other
        };

        return await this.createRequest(request);
    }

    static async request(url, method, other={}) {
        let request = {
            method: method,
            url: url,
            ...other
        };
        return await this.createRequest(request);
    }

    static async createRequest(request) {
        try {
            let _request = this.beforeRequest ? this.beforeRequest(request) : request;
            let resp = await axios.request(_request);
            return this.beforeResponse ? this.beforeResponse(resp) : resp;
        }
        catch (error) {
            //当设置了onError时过滤错误类型
            if (this.onError && error.isAxiosError) {
                return this.onError(error);
            }
            else {
                throw error;
            }
        }
    }
}