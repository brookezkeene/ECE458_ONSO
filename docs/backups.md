# Restoring from a backup
Our backup system can be accessed via the included script (see `/scripts/restore_backup.sh`). You will be prompted for a password to the backup system upon executing this script. In the interest of security, this password has not been included directly in the repository. Please [reach out to Cannon Palms via email](mailto:lcp27@duke.edu?subject=ECE458: Backup system credentials) and the credentials will be shared with you via LastPass.

## Using the backup restoration script
Example:
```bash
cd scripts

# make the script executable
chmod +x ./restore_backup.sh

# restore from a daily backup taken on March 25, 2020
./restore_backup.sh daily 20200325

# restore from a daily backup taken on March 23, 2020
./restore_backup.sh daily 20200323

# restore from a weekly backup taken on Sunday, March 15, 2020
./restore_backup.sh weekly 20200315

# restore from a monthly backup taken on June 1, 1970
./restore_backup.sh monthly 19700601
```

## Notes
* Only the 7 most recent daily backups, 4 most recent weekly backups, and 12 most recent monthly backups are available for restoration
* Weekly backups are made on midnight each Sunday
* Monthly backups are made on midnight on the 1st of each month
* All daily, weekly, and monthly backups are made at midnight EDT
* The database restoration script should not be run from Git Bash! Use a \*nix machine or WSL (Windows Subsystem for Linux). If you see strange output regarding ssh connection multiplexing, you did not follow this note!
