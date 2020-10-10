import pyrebase
from flask import Flask
import jsonify

#app = Flask('knightclub.online/api')

#@app.route('/')
#def home():
    #return jsonify("Hello from flask app.py file")

#app.run()

firebaseConfig={ 
    'apiKey': "AIzaSyDZL3Mxku1OorfBMjjAU2HSoDMEXHo13VY",
    'authDomain': "knight-club-36b05.firebaseapp.com",
    'databaseURL': "https://knight-club-36b05.firebaseio.com",
    'projectId': "knight-club-36b05",
    'storageBucket': "knight-club-36b05.appspot.com",
    'messagingSenderId': "18563193236",
    'appId': "1:18563193236:web:4f5511f45bf70b23fad781",
    'measurementId': "G-153N36MQL3"
}

# creates a firebase object
firebase = pyrebase.initialize_app(firebaseConfig)
auth = firebase.auth()
database = firebase.database()

FALSE = -1

# login 
def login():
    email = input("Enter your email: ")
    password = input("Enter your password: ")

    try:
        auth.sign_in_with_email_and_password(email, password)
    except:
        return FALSE

# signUp
def signUp():
    email = input("Enter your email: ")
    password = input("Enter your password: ")
    confirmpass = input("Confirm your password: ")

    if confirmpass == password:
        try:
            auth.create_user_with_email_and_password(email, password)
            print("User created!")
        except:
            print("Username is already taken.")

# database access
# .set, .push
# .push to create
# .set to edit 

def userExists(name):
    player = database.child('Users').get()
    for user in player.each():
        if user.key() == name:
            return 1
        
    return 0

def createNewPlayer (username, stuID):
    username = username.lower()

    if userExists(username) == 1:
        return FALSE
    
    data = {'Currency': 0, 'ID': stuID, 'pet': 'No', 'HeadOn': 'No',
            'College': 'Blank', 'ShirtOn': 'No', 'Major': 'Undecided'}
    database.child('Users').child(username).set(data)
    database.child('Users').child(username).child('Head').update({'Head' : 0})
    database.child('Users').child(username).child('Shirt').update({'Shirts' : 0})
    database.child('Users').child(username).child('Friends').update({'Friends': 0})
    
# this adds a user to database and set their status to 1
# we can use this as offline but the issue is if you add a friend you already have it sets them to offline
def addFriend(username, friendName):

    flag = 0
    if  userExists(friendName):
        database.child('Users').child(friendName).child('Friends').update({username: 1})
        database.child('Users').child(username).child('Friends').update({friendName: 1})

        # increment number of friends by one
        #path.update({'Friends': path.val()['Friends'] + 1})
        flag += 1
            
    if flag == 0:
        return FALSE
        
def addHead(username, itemName):
    database.child('Users').child(username).child('Head').update({itemName: 1})

def addShirt(username, itemName):
    database.child('Users').child(username).child('Shirt').update({itemName: 1})


def itemOwned(username, itemType, itemName):
    listy = database.child('Users').child(itemType).get()
    for item in listy.each():
        if item.key() == itemName:
            return 1
        
    return 0

def balanceCheck(username):
    money = database.child('Users').child(username).get()
    return money.val()['Currency']

def setCollege(username, ans):
    database.child('Users').child(username).set({'College': ans})

def getCollege(username):
    ans = database.child('Users').child(username).get()
    return ans.val()['College']

def selectShirt(username, item):
    if itemOwned(username, 'ShirtOn', item):
        data = {'shirt': item}
        database.child('Users').set(data)
    else:
        return FALSE

def selectHat(username, item):
    if itemOwned(username, 'HeadOn', item):
        data = {'shirt': item}
        database.child('Users').set(data)
    else:
        return FALSE

def removeUser(username):
    if userExists(username):
        database.child("Users").child(username).remove()
    else:
        return FALSE
