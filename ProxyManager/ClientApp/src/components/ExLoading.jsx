import React from 'react';
import { message } from 'antd';
import {StarOutlined,ReloadOutlined} from '@ant-design/icons';
import './ExLoading.css';

export default class ExLoading extends React.Component{
    static instance;
    constructor(props) {
        super(props);
        this.state = {
            show: false,
            icon: false,
            loadingMessage: "加载中..."
        }
    }
    componentWillMount() {
        if (!ExLoading.instance) {
            ExLoading.instance = this;
        }
        else {
            throw new Error("该组件只能渲染一次!");
        }
    }
    componentWillUnmount() {
        ExLoading.instance = undefined;
    }
    render() {
        if (this.state.show) {
            return (
                <div id="exloading" className="ant-modal-mask">
                    <div className="mask">
                        <div className="mask-loading">
                            {this.state.icon ? (
                                <ReloadOutlined spin style={{ fontSize: 18 }}/>
                            ) : (null)}
                            <span style={{ marginLeft: 10 }}>{this.state.loadingMessage}</span>
                        </div>
                    </div>
                </div>)
        }
        return (null);
    }
    static show = (showIcon = false, message = "Loading...") => {
        if (ExLoading.instance) {
            ExLoading.instance.setState({
                show: true,
                icon: showIcon,
                loadingMessage: message
            });

            return () => {
                ExLoading.instance.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }

    static hide = () => {
        if (ExLoading.instance) {
            ExLoading.instance.setState({
                show: false,
                icon: false
            });
        }
    }

    static info = (msg) => {
        if (ExLoading.instance) {
            var hide = message.info(msg, 0);
            ExLoading.instance.setState({
                show: true,
                icon: false
            });

            return () => {
                hide();
                ExLoading.instance.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }

    static error = (msg) => {
        if (ExLoading.instance) {
            var hide = message.error(msg, 0);
            ExLoading.instance.setState({
                show: true,
                icon: false
            });

            return () => {
                hide();
                ExLoading.instance.setState({
                    show: false,
                    icon: false,
                    loadingMessage: "加载中..."
                });
            };
        }
        return () => { };
    }
}