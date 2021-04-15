echo 'redialing...'
sleep 1
ifdown ppp0
sleep 1
ifup ppp0
sleep 1
service tinyproxy restart