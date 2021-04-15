import Mock from 'mockjs2'
export default [
    // Mock.mock('/api/nodemanager/get-all-nodes', 'get', {
    //     "success": true,
    //     "date": "2021-01-01 09:25:11",
    //     "data": [
    //         {
    //             "id": "1",
    //             "name": "节点1",
    //             "ip": "188.188.11.121",
    //             "port": 8080,
    //             "traffic":"11MB",
    //             "state": "正常"
    //         },
    //         {
    //             "id": "2",
    //             "name": "节点2",
    //             "ip": "188.188.11.121",
    //             "port": 8080,
    //             "traffic":"21MB",
    //             "state": "拨号中"
    //         },
    //         {
    //             "id": "3",
    //             "name": "节点3",
    //             "ip": "188.188.11.121",
    //             "port": 8080,
    //             "traffic":"1GB",
    //             "state": "正常"
    //         }
    //     ]
    // }),
    // Mock.mock(/'\/api\/nodemanager\/re-dial.*'/ ,'get', {
    //     "success": true,
    //     "date": "2021-01-01 09:25:11",
    //     "data": null
    // }),
    // Mock.mock('/api/auth/login', "post", option => {
    //     if(option.body=='{"userName":"admin","password":"admin"}'){
    //         return {
    //             "success": true,
    //             "date": "2021-01-01 09:25:11",
    //             "data": {"token":"mock_token"},
    //             "message":"登录成功"
    //         }
    //     }
    //     else{
    //         return {
    //             "success": true,
    //             "date": "2021-01-01 09:25:11",
    //             "data": {},
    //             "message":"用户名或密码错误"
    //         }
    //     }

    // }),
    // Mock.mock('/api/check-login', "get", () => {
    //     if (localStorage.getItem("token")) {
    //         return {
    //             "success": true,
    //             "date": "2021-01-01 09:25:11",
    //             "data": {
    //             }
    //         }
    //     }
    //     return {
    //         _status: 401
    //     }
    // }),
    // Mock.mock(/\/api\/nodemanager\/log.*/,'get',{
    //     "success": true,
    //     "date": "2021-01-01 09:25:11",
    //     "data":[
    //         {"timestamp":1618280968383,"message":"日志1"},
    //         {"timestamp":1618280968383,"message":"日志2"},
    //         {"timestamp":1618280968383,"message":"日志3"},
    //         {"timestamp":1618280968383,"message":"日志4"}
    //     ]
    // }),
    // Mock.mock(/\/api\/api-info/,'get',{
    //     "success": true,
    //     "date": "2021-01-01 09:25:11",
    //     "data":[
    //         {"name":"api/client/get","content":"随机获得一条HTTP代理节点"},
    //         {"name":"api/client/get-all","content":"随机全部HTTP代理节点"},
    //         {"name":"api/client/redial?id=","content":"强制拨号一个代理节点"},
    //         {"name":"api/client/redial-all","content":"强制拨号所有节点"}
    //     ]
    // }),
    // Mock.mock(/\/api\/get-config/,'get',{
    //     "success": true,
    //     "date": "2021-01-01 09:25:11",
    //     "data":{
    //         "heart_interval": 10,
    //         "auto_redial": true,
    //         "redial_interval":3600
    //     }
    // }),
    // Mock.mock(/\/api\/save-config/,'post',option=>{
    //     return {
    //         "success": true,
    //         "date": "2021-01-01 09:25:11",
    //         "data":{}
    //     }
    // })
]