//
//  InternationalControl.h
//  RFID_ios
//
//  Created by hjl on 2018/10/22.
//  Copyright © 2018年  . All rights reserved.
//

#import <Foundation/Foundation.h>

@interface InternationalControl : NSObject

+(NSBundle *)bundle;//获取当前资源文件

+(void)initUserLanguage;//初始化语言文件

+(NSString *)userLanguage;//获取应用当前语言

+(void)setUserlanguage:(NSString *)language;//设置当前语言

@end
