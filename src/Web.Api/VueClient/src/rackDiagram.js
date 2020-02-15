import rackRepository from '@/repositories/mock/rack';

export default {

    async createRacksByRows(start, end) {

        var racksInRange;
        
        racksInRange = await rackRepository.findInRange(start, end);
       
        const { rowLetter: startRow, rackNumber: startCol } = rackRepository.splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = rackRepository.splitAddress(end);
        var racksByRows = [];

        var rackIndex = 0;
        for (var r = startRow.charCodeAt(0); r <= endRow.charCodeAt(0); r++) {
            var row = {
                rowLetter: String.fromCharCode(r),
                racks: [],
            };
            for (var col = startCol; col <= endCol; col++) {

                var slot = this.createSlot(racksInRange[rackIndex].instances);
                var rack = {
                    address: racksInRange[rackIndex].address,
                    slots: slot
                };
                row.racks.push(rack);
                rackIndex++;
            }
            racksByRows.push(row);
        }

        return racksByRows;
    },
    createSlot(instances) {
        //creating base rows of a single rack diagram
        var rows = [];
        for (var i = 0; i < 42; i++) {
            var row = {
                rackU: i + 1,
                value: '',
                style: '',
            };
            rows.push(row);
        }
        //filling the rack diagram with instance data  
        var instances_length = Object.keys(instances).length;
        for (var j = 0; j < instances_length; j++) {
            var rackU = instances[j].rackPosition - 1;
            var color = instances[j].model.displayColor;
            rows[rackU].style = { color: 'black', backgroundColor: color };
            rows[rackU].value = { text: instances[j].model.vendor + ' ' + instances[j].model.modelNumber + ' ' + instances[j].hostname, id: instances[j].id };

            var model_height = instances[j].model.height
            for (var k = 1; k < model_height; k++) {
                var position = rackU + k;
                rows[position].style = { color: 'black', backgroundColor: color };
            }
        }
        return rows.reverse();
    },
}