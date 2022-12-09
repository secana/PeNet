Information to:
1. pidgin_manipulated_GroupIconDirectory.exe
- Contains 4 GroupIconDirectories with [9,0,2,9,0] GroupIconDirectoryEntries.
- Thereby consist:
    - The first out of the normal pidgin.exe GroupIconDirectory.
    - The second GroupIconDirectory out of random bytes.
    - The third GroupIconDirectory out of a shorted version of the first GroupIconDirectory with only 2 GroupIconDirectoryEntries.
    - The fours GroupIconDirectory out of a with random byte extended version of the first GroupIconDirectory.
    - The fives GroupIconDirectory out of a shorted version of the first GroupIconDirectory with only 4 bytes.

2. pidgin_manipulated_GroupIconDirectory_withNullIcon.exe
- Contains 5 GroupIconDirectories with [9,0,2,9,0] GroupIconDirectoryEntries and a additionally dummy Icon with ID 17473.
- Thereby consist:
    - The first out of the normal pidgin.exe GroupIconDirectory.
    - The second GroupIconDirectory out of random bytes.
    - The third GroupIconDirectory out of a shorted version of the first GroupIconDirectory.
    - The fours GroupIconDirectory out of a with random byte extended version of the first GroupIconDirectory.
    - The fives GroupIconDirectory out of a shorted version of the first GroupIconDirectory with less then one whole GroupIconDirectoryEntry.

3. pidgin_manipulated_Icons.exe:
- Contains 15 Icons
- Thereby are:
  - Icon 1-9 the normal Icons from pidgin.exe [Displayable, Extractable]
  - Icon 10 a shortened version of Icon 9 [Extractable]
  - Icon 11 a version of Icon 9 with random byte in his Header [Extractable]
  - Icon 12 a shortened version of Icon 9 which is shorter as the Header
  - Icon 13 a with random bytes extended version of Icon 9 [Displayable, Extractable]
  - Icon 14 has only 1 byte
  - Icon 15 contains a image consisting out of random bytes with the header of Icon 9. [Displayable, Extractable]