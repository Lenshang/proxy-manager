import React from 'react';
import { PageHeader, Form, Input, Button, Select, InputNumber, Switch,notification } from 'antd';
import { Link } from 'react-router-dom';
import ExLoading from '../components/ExLoading';
import api from '../api/ConfigPage';
import "./ConfigPage.css";
const { Option } = Select;

export default class ConfigPage extends React.Component {
    formRef = React.createRef();
    constructor(props) {
        super(props);
        this.state = {
            auto_redial: false
        }
    }
    componentDidMount() {
        ExLoading.show(true);
        api.getConfig().then(resp=>{
            ExLoading.hide();
            if(!resp.success){
                notification['error']({
                    message: '获取数据失败!'
                })
            }
            this.setState({
                auto_redial: resp.data.auto_redial==="True"
            })
            this.formRef.current.setFieldsValue({
                heart_interval: parseInt(resp.data.heart_interval),
                auto_redial: resp.data.auto_redial==="True",
                redial_interval:parseInt(resp.data.redial_interval)
            });
        });
    }
    onFinish = (values) => {
        ExLoading.show(true);
        api.saveConfig(values).then(resp=>{
            ExLoading.hide();
            if(!resp.success){
                notification['error']({
                    message: '保存失败! 理由:'+resp.message
                })
                return;
            }
            notification['success']({
                message: '保存成功!'
            })
        });
        console.log(values);
    };
    render() {
        const layout = {
            labelCol: { span: 6 },
            wrapperCol: { span: 20 },
        };
        const tailLayout = {
            wrapperCol: {
                sm: { offset: 6, span: 20 },
                xs: { offset: 0, span: 18 },
            },
        };
        return (
            <div id="config-page">
                <PageHeader style={{ backgroundColor: "white" }} className="site-page-header" title="设置" subTitle="全局配置">
                </PageHeader>
                <Form style={{ marginTop: 20, padding: 20, backgroundColor: "white" }} {...layout} ref={this.formRef} name="control-ref" onFinish={this.onFinish}>
                    <Form.Item name="heart_interval" label="节点心跳间隔" rules={[{ required: true }]}>
                        <InputNumber />
                    </Form.Item>
                    <Form.Item name="auto_redial" label="定时自动拨号">
                        <Switch checked={this.state.auto_redial} onChange={c => this.setState({ auto_redial: c })} />
                    </Form.Item>
                    {this.state.auto_redial ? (
                        <Form.Item name="redial_interval" label="自动拨号间隔" rules={[{ required: true }]}>
                            <InputNumber />
                        </Form.Item>
                    ) : (<></>)}
                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">
                            保存
                        </Button>
                    </Form.Item>
                </Form>
            </div>);
    }
}