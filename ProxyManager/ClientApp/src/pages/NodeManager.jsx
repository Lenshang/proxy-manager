import React from 'react';
import { Card, Row, Col, message, notification } from 'antd';
import { Link } from 'react-router-dom';
import {dateFormat} from '../utils/CommonUtils';
import ExLoading from '../components/ExLoading';
import api from "../api/NodeManager";
import "./NodeManager.css";
class NodeManager extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            nodes: []
        }
    }
    componentDidMount() {
        this.getNodes();
        this.interval = setInterval(() => this.getNodes(), 1000);
    }
    componentWillUnmount() {
        clearInterval(this.interval);
    }
    getNodes = async () => {
        var data = await api.get_all_nodes();
        this.setState({
            nodes: data
        });
    };

    reDial = async (id) => {
        ExLoading.show(true);
        var success = await api.re_dial(id)
        if (success) {
            notification['success']({
                message: '操作成功!'
            })
        }
        else {
            notification['error']({
                message: '操作失败!'
            })
        }
        ExLoading.hide();
    }
    getFormatDate=(dt)=>{
        let date = new Date(dt)
        return dateFormat("YYYY-mm-dd HH:MM:SS", date)
    };
    render() {
        const TaskComponents = () => {
            return this.state.nodes.map((item) => {
                return (
                    <Col key={item.id} xs={24} sm={24} lg={12} xl={8} xxl={6}>
                        <Card hoverable key={item.id} title={item.TaskName} actions={[
                            (<div onClick={() => { this.reDial(item.id) }}>强制拨号</div>),
                            (<Link to={'/task/log?taskid=' + item.id}>日志</Link>),
                        ]}>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>名称 :</div>{item.name}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>状态 :</div>{item.state}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>IP :</div>{item.ip}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>端口 :</div>{item.port}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>位置 :</div>{item.location}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>获取次数 :</div>{item.use_count}</div>
                            <div style={{ display: "flex" }}><div style={{ width: "30%", textAlign: "right", marginRight: 10 }}>上次心跳 :</div>{this.getFormatDate(item.lastHBDate)}</div>
                        </Card>
                    </Col>
                )
            });
        }
        return (
            <div id="app-manager">
                <Row gutter={[16, 24]}>
                    {TaskComponents()}
                </Row>
            </div>
        )
    }
}

export default NodeManager;