import http from 'k6/http';
import { check } from 'k6';

export const options = {
  stages:[
  { duration: '30s', target: 20 },
]
};
export default function () {
    const params = {
        headers: {
            'accept': '*/*',
            'accept-encoding': 'gzip, deflate, br',
            'api-key': 'strongpasswordmino',
        },
    };
    const res = http.get('https://localhost:9200/indexes/ordersindex/docs?api-version=2021-04-30-Preview&search=1-373-659-7215&$top=10', params);
    check(res, {
      'is status 200': (r) => r.status === 200,
    });
}
