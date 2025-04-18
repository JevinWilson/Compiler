typedef void*                   HANDLE;
 typedef unsigned                DWORD; //32 bits
 typedef signed long long        int64_t;
 typedef unsigned long long      uint64_t;
 _Static_assert( sizeof(DWORD)       == 4, "DWORD bad");
 _Static_assert( sizeof(int64_t)     == 8, "int64_t bad");
 #define NULL ((void*)0)
 
 
 typedef struct StackVar_ {
     int64_t storageClass;
     int64_t value;
 } StackVar;
 
 HANDLE stdin;
 HANDLE stdout;
 
 HANDLE GetStdHandle(unsigned int);
 int WriteFile(HANDLE, char*, int, DWORD*, void*);
 int ReadFile(HANDLE, char*, int, DWORD*, void*);
 HANDLE CloseHandle(void*);
 unsigned toBin(uint64_t, char[64]);
 unsigned toDecimal(uint64_t, char[20]);
 unsigned toHex(uint64_t, char[16]);
 
 
 //runtime init
 __attribute__((ms_abi)) void rtinit(){
     stdin = GetStdHandle(0xfffffff6);
     stdout = GetStdHandle(0xfffffff5);
 }
 
 __attribute__((ms_abi)) int putc(StackVar stk[])
 {
     DWORD count;
     char v = stk[0].value;
     int rv = WriteFile( stdout, &v, 1, &count, NULL );
     if(rv == 0 || count == 0 )
         return 0;
     else
         return 1;
 }
 
 __attribute__((ms_abi)) int getc() //change from void to int later
 {
     DWORD count;
     char v;
     int rv = ReadFile(stdin, &v, 1, &count, NULL);
     int wv = WriteFile(stdout, &v, 1, &count, NULL);
     return (int)v;
 }
 
 __attribute__((ms_abi)) void newline()
 {
     DWORD count;
     char v = '\n';
     WriteFile( stdout, &v, 1, &count, NULL );
 }
 
 __attribute__((ms_abi)) int putv(StackVar stk[])
 {
 
     int64_t x = stk[0].value;
     int64_t y = stk[1].value;
 
     if(y != 2 && y != 10 && y != 16)
         return 0;
     else{
         DWORD count;
         switch (y)
         {
         case 2:
             char v[64];
             unsigned vee = toBin(x,v);
             WriteFile( stdout, v, vee, &count, NULL );
             break;
         case 10:
             char z[20];
             unsigned zee = toDecimal(x,z);
             WriteFile( stdout, z, zee, &count, NULL );
             break;
         case 16:
             char c[16];
             unsigned cee = toHex(x,c);
             WriteFile( stdout, c, cee, &count, NULL );
             break;
         default:
             break;
         }
         return 1;
     }
 }
 
 //runtime cleanup
 __attribute__((ms_abi)) void rtcleanup()
 {
     CloseHandle(stdin);
     CloseHandle(stdout);
 }
 
 unsigned toBin(uint64_t number, char output[64]){
     uint64_t mask = 0x8000000000000000ULL;
     if( number == 0 ){
         output[0]=0;
         return 1;
     }
     int oo=0;
     for(int i=0;i<64;++i,mask>>=1){
         if( mask & number ){
             output[oo++] = '1';
         } else {
             if( oo > 0 )
                 output[oo++] = '0';
         }
     }
     return oo;
 }
 
 unsigned toDecimal(uint64_t x, char output[20])
 {
     if( x == 0 ){
         *output = '0';
         return 1;
     }
     //2**64        = 18446744073709551616
     uint64_t place = 10000000000000000000ULL;
     int oo=0;
     while(place > 0 ){
         int64_t quotient = x/place;
         if( quotient || oo > 0 ) {
             output[oo++] = '0' + quotient;
         }
         x = x - quotient * place;
         place = place/10;
     }
     return oo;
 }
 
 unsigned toHex(uint64_t x, char output[16]){
     unsigned shiftcount = 60;
     unsigned oo=0;
     const char* digits = "0123456789abcdef";
     if( x == 0 ){
         output[0]='0';
         return 1;
     }
     for(unsigned i=0;i<16;++i){
         unsigned j = (unsigned)((x>>shiftcount) & 0xf );
         if( oo > 0 || j )
             output[oo++] = digits[j];
         shiftcount -= 4;
     }
     return oo;
 }