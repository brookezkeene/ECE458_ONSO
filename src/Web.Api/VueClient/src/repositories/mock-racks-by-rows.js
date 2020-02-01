﻿const slots = [
    { rackU: 1, value: '', style: '' },
    { rackU: 2, value: '', style: '' },
    { rackU: 3, value: '', style: '' },
    { rackU: 4, value: 'Dell r710 server1', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 5, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 6, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 7, value: '', style: { color: 'black', backgroundColor: '#82E0AA' } },
    { rackU: 8, value: '', style: '' },
    { rackU: 9, value: '', style: '' },
    { rackU: 10, value: '', style: '' },
    { rackU: 11, value: '', style: '' },
    { rackU: 12, value: 'Dell r710 server2', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 13, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 14, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 15, value: '', style: { color: 'black', backgroundColor: '#C39BD3' } },
    { rackU: 16, value: '', style: '' },
    { rackU: 17, value: '', style: '' },
    { rackU: 18, value: '', style: '' },
    { rackU: 19, value: '', style: '' },
    { rackU: 20, value: '', style: '' },
    { rackU: 21, value: '', style: '' },
    { rackU: 22, value: '', style: '' },
    { rackU: 23, value: '', style: '' },
    { rackU: 24, value: '', style: '' },
    { rackU: 25, value: '', style: '' },
    { rackU: 26, value: '', style: '' },
    { rackU: 27, value: '', style: '' },
    { rackU: 28, value: '', style: '' },
    { rackU: 29, value: '', style: '' },
    { rackU: 30, value: '', style: '' },
    { rackU: 31, value: '', style: '' },
    { rackU: 32, value: '', style: '' },
    { rackU: 33, value: '', style: '' },
    { rackU: 34, value: '', style: '' },
    { rackU: 35, value: '', style: '' },
    { rackU: 36, value: '', style: '' },
    { rackU: 37, value: '', style: '' },
    { rackU: 38, value: '', style: '' },
    { rackU: 39, value: '', style: '' },
    { rackU: 40, value: '', style: '' },
    { rackU: 41, value: '', style: '' },
    { rackU: 42, value: '', style: '' },
].reverse();

const rows = [
    {
        rowLetter: 'A',
		racks: [
			{
				address: 'A1',
				slots: slots
			},
			{
				address: 'A2',
				slots: slots
			},
			{
				address: 'A3',
				slots: slots
            }
        ],
    },
    {
        rowLetter: 'B',
		racks: [
            {
                address: 'B1',
                slots: slots
            },
            {
                address: 'B2',
                slots: slots
            },
            {
                address: 'B3',
                slots: slots
            }
        ],
    }
];

export default rows;