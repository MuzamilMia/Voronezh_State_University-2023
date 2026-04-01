# Задача:3A
"""
Тюрьма хранит номер заключенного по его ФИО с помощью словаря. Заключенных
можно сажать и отпускать на все четыре стороны, выводить номер зека по ФИО,
выводить всю базу (в столбик пары ФИО-номер) для администрации. Также должна
быть возможность напечатать суммарное количество заключенных на содержании.
"""
# Автор: Mia Muzamil Ahmad


N = 1000  # Constant for the prisoners amount.

#Main Menu
def print_menu():
    print("\nMenu:")
    print("1. Insert the prison")
    print("2. Delete the prison")
    print("3. Find the prison number by name")
    print("4. Show all the prisoners")
    print("5. Show all the amount of the prisoners")
    print("6. Exit")


def add_prisoner(prison_db, fio, number):
    """Function for adding the prisoner"""
    if len(prison_db) < N:
        prison_db[fio] = number
        return True
    else:
        return False

def remove_prisoner(prison_db, fio):
    """Function for deleting the prisoners."""
    if fio in prison_db:
        del prison_db[fio]
        return True
    else:
        return False


def find_prisoner(prison_db, fio, number):
    """ Function for searching the prisoner by name with the number ."""
    if number:
        return True
    else:
        return False

def show_all_prisoners(prison_db):
    """Function for showing all the prisoners."""
    if prison_db:
        print("List of all prisoners:")
        for fio, number in prison_db.items():
            print(f"{fio} - {number}")
    else:
        print("Empty.")


def count_prisoners(prison_db):
    """Function to show all the prisoners."""
    print(f"All (together) prisoners amount: {len(prison_db)}")

def main():
    """Main Function"""
    prison_db = {}  #Dictionary for usage

    while True:
        print_menu()
        choice = input("Choose the operation (1-6): ")

        if choice == '1':
            fio = input("Insert the prisoner name: ")
            number = input("Insert the number of the prisoner: ")
            if add_prisoner(prison_db,fio, number):
                print(f"Prisoner {fio} added to the dictionary by number: {number}.")
            else:
                print(f"Dictionary is Full. Maximum {N} prisoners.")

        elif choice == '2':
            fio = input("Insert the name of the prisoner for removing: ")
            if remove_prisoner(prison_db,fio):
                print(f"Prisoner {fio} removed from the dictionary.")
            else:
                print(f"Prisoner with name {fio} not found.")

        elif choice == '3':
            fio = input("Insert the name of the prisoner: ")
            number = prison_db.get(fio)
            if find_prisoner(prison_db, fio, number):
                print(f"prisoner {fio} has the number- {number}.")
            else:
                print(f"Prisoner with the name {fio} not found.")

        elif choice == '4':
            show_all_prisoners(prison_db)
        elif choice == '5':
            count_prisoners(prison_db)
        elif choice == '6':
            print("Exit.")
            break
        else:
            print("Error, Try again")


if __name__ == "__main__":
    main()