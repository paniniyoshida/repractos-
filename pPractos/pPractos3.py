# -*- coding: cp1251 -*-
import random
import json
import csv

def save_game_data(data):
    with open('data.json', 'w') as file:
        json.dump(data, file)

def load_game_data():
    try:
        with open('data.json', 'r') as file:
            data = json.load(file)
    except FileNotFoundError:
        data = {}
    return data

def update_csv(data):
    with open('game_data.csv', 'w', newline='') as file:
        writer = csv.writer(file)
        writer.writerow(["�����", "����"])
        for player, role in data.items():
            writer.writerow([player, role])

def start_game(n):
    mafia = random.choice(n)
    b = {m: "�����" if m == mafia else "��������" for m in n}
    data = load_game_data()
    data.update(b)
    save_game_data(data)
    update_csv(data)
    print("������ ����!")
    print("�����:", mafia)
    print("��������:", [player for player in n if player != mafia])



    players = []
    b = ['�����', '������ ������', '������ ������','������ ������','������ ������','������ ������']
    random.shuffle(b)
    
    # ������� ������� � ��������� ������� ������������ ����
    for i in range(len(b)):
        player = {'����': b[i], '���': True}
        players.append(player)
    
    # ���� ����
    while True:
        for i in range(len(players)):
            print(f"����� {i+1}")
        
        # ������ ����� ������ 
        vote = int(input("���� ������ ���������? (������� ����� ������): "))
        
        # �������� ���������� �����
        if vote < 1 or vote > len(players):
            print("�������� ����� ������, ���������� ��� ���.")
            continue
        
        # �������� ���� ���������� ������
        if players[vote-1]['����'] == '�����':
            print("����� �����", vote, "����������� � �����!")
            break
        else:
            print("����� �����", vote, "�� ����������� � �����!")
        
        # �������� �� ������ ����� �� ������
        mafia_count = 0
        citizens_count = 0
        for player in players:
            if player['���']:
                if player['����'] == '�����':
                    mafia_count += 1
                else:
                    citizens_count += 1
        if mafia_count == 0:
            print("������ ������ ��������!")
            break
        elif mafia_count >= citizens_count:
            print("����� ��������!")
            break

def delete_save_data():
    with open('data.json', 'w') as file:
        file.write("")
    with open('game_data.csv', 'w', newline='') as file:
        file.write("")

def main_menu():
    while True:
        print("1. ������ ����")
        print("2. ������� ����������")
        print("3. �����")
        choice = input("������� ���� �����: ")
        if choice == "1":
            players = input("������� ����� ������� (����� �������): ").split(",")
            start_game(players)
        elif choice == "2":
            delete_save_data()
            print("Save data deleted.")
        elif choice == "3":
            break
        else:
            print("�������� �����. ����������, ���������� �����.")

if __name__ == "__main__":
    main_menu()

