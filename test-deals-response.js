// Simple test to check deals API response structure
const axios = require('axios');

async function testDealsAPI() {
    try {
        console.log('Testing Deals API...');
        
        const response = await axios.get('http://localhost:5215/api/deals/listing', {
            params: { PageSize: 3 }
        });
        
        console.log('=== API Response Structure ===');
        console.log('Response status:', response.status);
        console.log('Total items:', response.data.totalItems);
        console.log('Items count:', response.data.items.length);
        
        if (response.data.items.length > 0) {
            console.log('\n=== First Deal Item ===');
            const firstDeal = response.data.items[0];
            console.log('Deal structure:');
            console.log(JSON.stringify(firstDeal, null, 2));
            
            console.log('\n=== Field Analysis ===');
            console.log('baseDealId:', firstDeal.baseDealId);
            console.log('title:', firstDeal.title);
            console.log('status:', firstDeal.status);
            console.log('dealType:', firstDeal.dealType);
            console.log('country:', firstDeal.country);
            console.log('dealValue:', firstDeal.dealValue);
            
            console.log('\n=== Status Field Check ===');
            console.log('Status field exists:', 'status' in firstDeal);
            console.log('Status value:', firstDeal.status);
            console.log('Status is empty:', firstDeal.status === '' || firstDeal.status == null);
        }
        
    } catch (error) {
        console.error('Error testing API:', error.message);
        if (error.response) {
            console.error('Response status:', error.response.status);
            console.error('Response data:', error.response.data);
        }
    }
}

testDealsAPI();
