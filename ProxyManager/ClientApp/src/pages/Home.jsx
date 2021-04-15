import React from 'react';
import { Descriptions, Badge, notification } from 'antd';
import api from '../api/Home';
export default class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: []
        }
    }
    componentDidMount() {
        api.getApiInfo().then(_data => {
            if (_data.success) {
                this.setState({ data: _data.data });
            }
            else if (_data.message) {
                notification['error']({
                    message: _data.message
                })
            }
            else {
                notification['error']({
                    message: "获取首页信息失败!未知错误!"
                })
            }
        });
    }
    render() {
        const getDescription = () => {
            return this.state.data.map(item => {
                return (
                    <Descriptions.Item key={item.name} label={"API："+item.name}>
                        {item.content}
                    </Descriptions.Item>);
            });
        }
        return (
            <div style={{ backgroundColor: "white", padding: 25 }}>
                <Descriptions column={2} title="运行信息" layout="vertical" margin bordered>
                    <Descriptions.Item key="节点总数" label="节点总数">
                        3
                    </Descriptions.Item>
                    <Descriptions.Item key="当日流量" label="当日流量">
                        999mb
                    </Descriptions.Item>
                </Descriptions>
                <Descriptions column={1} title="Api说明" layout="vertical" bordered>
                    {getDescription()}
                </Descriptions>
            </div>)
    }
}