import json

with open('getmarkets', 'r') as f:
	datas = json.load(f)
	for key in datas:
		print key
