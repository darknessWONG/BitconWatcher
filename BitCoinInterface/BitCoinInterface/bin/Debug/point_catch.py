import sys
sys.path.append('C:\\Python27\\Lib')

import urllib
import urllib2
import hashlib
import json



#respone = urllib2.urlopen("https://api.bitflyer.jp/")

def create_public_request(func_name, argu = None):
	url = 'https://api.bitflyer.jp/v1/' + func_name
	if argu != None:
		for key in argu:
			url += '?'
			url += key + '='
			url += argu[key]
	user_agent = 'Mozilla/4.0 (compatible; MSIE 5.5; Windows NT)'  
	#values = {'username' : 'cqc',  'password' : 'XXXX' }  
	headers = { 'User-Agent' : user_agent }  
	#data = urllib.urlencode(values)  
	request = urllib2.Request(url, None, headers)  
	return request

def get_json_from_request(request):
	response = urllib2.urlopen(request)
	page = response.read()
	new_json = json.loads(page)
	return new_json


class PublicApi:
	func_name = ""
	argu = {}
	def create_request(self):
		#do something
		return None


class Ticker(PublicApi):
	def __init__(self):
		PublicApi.func_name = "ticker"
		self.request = None
		self.json = None

	def create_request(self):
		self.request = create_public_request(PublicApi.func_name)

	def refresh(self):
		if self.request != None:
			self.json = get_json_from_request(self.request)

	def start(self):
		self.create_request()
		self.refresh()

	def get_ltp_val(self):
		if self.json == None:
			return None
		else:
			return self.json['ltp']

def Get_Ltp_Val():
	tricker = Ticker()
	tricker.start()
	return tricker.get_ltp_val()
	#print tricker.get_ltp_val()
	#tricker.refresh()
	#print tricker.get_ltp_val()