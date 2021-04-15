yum install tmux libevent libicu tinyproxy -y

vi /etc/tinyproxy/tinyproxy.conf
修改Port 8787
注释 #Allow 127.0.0.1
service tinyproxy restart
firewall-cmd --zone=public --add-port=8787/tcp --permanent
firewall-cmd --reload

chmod 777 redial.sh