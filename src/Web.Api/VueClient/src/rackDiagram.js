/* eslint-disable no-unused-vars, no-console */
import rackRepository from '@/repositories/rack';

const splitAddress = (address) => {
    if (!validAddress(address))
        return {};
    const groups = address.match(/^([a-zA-Z])(\d+)$/);
    return { rowLetter: groups[1].toUpperCase(), rackNumber: parseInt(groups[2]) }
}

const validAddress = (address) => {
    return /^[a-zA-Z]\d+$/.test(address);
}

export default {

    async createRacksByRows(start, end, id) {
        var racksInRange;
        
        racksInRange = await rackRepository.findInRange(start, end, id);
        console.log(racksInRange)
       
        const { rowLetter: startRow, rackNumber: startCol } = splitAddress(start);
        const { rowLetter: endRow, rackNumber: endCol } = splitAddress(end);
        var racksByRows = [];

        var rackIndex = 0;
        for (var r = startRow.charCodeAt(0); r <= endRow.charCodeAt(0); r++) {
            var row = {
                rowLetter: String.fromCharCode(r),
                racks: [],
            };
            for (var col = startCol; col <= endCol; col++) {

                var slot = this.createSlot(racksInRange[rackIndex].assets);
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
    createSlot(assets) {
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
        //filling the rack diagram with asset data  
        var assets_length = Object.keys(assets).length;
        for (var j = 0; j < assets_length; j++) {
            var rackU = assets[j].rackPosition - 1;
            var color = assets[j].model.displayColor;
            rows[rackU].style = { color: 'black', backgroundColor: color };
            rows[rackU].value = { text: assets[j].model.vendor + ' ' + assets[j].model.modelNumber + ' ' + assets[j].assetNumber, id: assets[j].id };

            var model_height = assets[j].model.height
            for (var k = 1; k < model_height; k++) {
                var position = rackU + k;
                rows[position].style = { color: 'black', backgroundColor: color };
            }
        }
        return rows.reverse();
    },
}