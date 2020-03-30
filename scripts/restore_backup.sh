#!/bin/bash

vcm_host="vcm-12801"

# arg checking
if [[ ! $1 =~ ^(daily|weekly|monthly)$ ]]
then
        echo Invalid argument: $1
        exit 1
fi

# configure ssh multiplexing
ssh_config_file="$HOME/.ssh/config"
host="$vcm_host.vm.duke.edu"
ssh_config=$(cat << EOF
Host $host
        ControlMaster auto
        ControlPath $HOME/.ssh/%r@%h:%p
        ControlPersist 300s
EOF
)

echo
echo "NOTE: This script modifies your ssh_config (found at ~/.ssh/config) in order to prevent multiple password
prompts during the restoration process. It only affects your ssh connections to the backup system, but if this is
something that you would like to remove after running regardless, feel free to do so."
echo

if test -f $ssh_config_file
then
        if ! grep -q "$host" "$ssh_config_file"
       	then
               	echo "$ssh_config" >> $ssh_config_file
        fi
else
        touch $ssh_config_file
        echo "$ssh_config" > $ssh_config_file
fi

dexec="sudo docker exec -i Web.Api.Infrastructure.SqlServer"
file_path="/var/opt/mssql/backup/$1/webapi_$2.bak"

if ssh vcm@$vcm_host.vm.duke.edu "$dexec test -f $file_path"
then
        echo "Found backup file: webapi_$2.bak"
        ssh vcm@$vcm_host.vm.duke.edu "$dexec ls -lh /var/opt/mssql/backup/$1 | grep $2 | xargs echo"

        echo

        read -p "Are you sure you would like to restore from webapi_$2.bak? (Y/n) " -n 1 -r

        echo # move to a new line

        if [[ ! $REPLY =~ ^[Yy]$ ]]
        then
                echo "Quitting..."
                exit 1
        fi

	echo Stopping web api...
	ssh -q vcm@$vcm_host.vm.duke.edu "sudo docker stop src_web.api_1"
	echo Done.


        # the user has confirmed that he/she would like to restore from this backup. Proceed...
        restore_sql="RESTORE DATABASE [webapi] FROM DISK = '$file_path' WITH MOVE 'webapi_log' TO '/var/opt/mssql/data/webapi_log.ldf', MOVE 'webapi' TO '/var/opt/mssql/data/webapi'"
        response=$(ssh vcm@$vcm_host.vm.duke.edu "$dexec /opt/mssql-tools/bin/sqlcmd -b \
                -S localhost -U sa -P Password_123 \
                -Q \"$restore_sql\"")

        if [ $? -ne 0 ] && echo "$response" | grep -q "has not been backed up"
        then
                echo
                read -p "The tail of the logs for this database contain work that has not been backed up. Are you sure you want to overwrite? (Y/n) " -n 1 -r
                echo
                echo

                if [[ ! $REPLY =~ ^[Yy]$ ]]
                then
                        echo "Quitting..."
			ssh -q vcm@$vcm_host.vm.duke.edu "sudo docker start src_web.api_1"
                        exit 1
                fi
                response=$(ssh vcm@$vcm_host.vm.duke.edu "$dexec /opt/mssql-tools/bin/sqlcmd -b \
                        -S localhost -U sa -P Password_123 \
                        -Q \"$restore_sql, REPLACE\" && exit 0 || exit 1")
                if [ $? -ne 0 ]
                then
                        echo An unknown error has occurred. Please notify an administrator.
			ssh -q vcm@$vcm_host.vm.duke.edu "sudo docker start src_web.api_1"
                        exit 1
                fi
        fi

        echo $response
else
        echo Backup file not found for date $2. See list of available $1 backup files below.
        echo

        echo Available $1 backups:
        ssh vcm@vcm-13156.vm.duke.edu "$dexec ls -1t /var/opt/mssql/backup/$1"

        exit 1
fi

echo Successfully restored database from backup.
ssh -q vcm@$vcm_host.vm.duke.edu "sudo docker start src_web.api_1"
exit 0


